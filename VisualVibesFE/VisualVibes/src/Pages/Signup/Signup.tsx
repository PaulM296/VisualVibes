import React, { useState } from 'react';
import './Signup.css';
import { Helmet } from 'react-helmet-async';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import { registerUser } from '../../Services/AuthenticationServiceApi';
import { Link } from 'react-router-dom';
import { TextField, FormControl, InputLabel, Select, MenuItem, FormHelperText, Button } from '@mui/material';
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';

const validationSchemaStep1 = Yup.object().shape({
    email: Yup.string().email('Invalid email').required('Email is required'),
    username: Yup.string().required('Username is required'),
    password: Yup.string().min(8, 'Password must be at least 8 characters').required('Password is required'),
    confirmPassword: Yup.string().oneOf([Yup.ref('password'), undefined], 'Passwords must match').required('Confirm Password is required'),
});

const validationSchemaStep2 = Yup.object().shape({
    firstName: Yup.string().required('First name is required'),
    lastName: Yup.string().required('Last name is required'),
    dateOfBirth: Yup.date().required('Date of birth is required'),
    role: Yup.string().oneOf(['admin', 'user'], 'Role must be either admin or user').required('Role is required'),
    bio: Yup.string(),
    image: Yup.mixed().nullable()
});

const Signup: React.FC = () => {
    const [step, setStep] = useState(1);
    const [initialValuesStep2, setInitialValuesStep2] = useState({
        email: '',
        username: '',
        password: '',
        confirmPassword: '',
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        role: '',
        bio: '',
        image: null
    });

    const formikStep1 = useFormik({
        initialValues: {
            email: '',
            username: '',
            password: '',
            confirmPassword: ''
        },
        validationSchema: validationSchemaStep1,
        onSubmit: (values) => {
            setInitialValuesStep2((prevValues) => ({
                ...prevValues,
                email: values.email,
                username: values.username,
                password: values.password,
                confirmPassword: values.confirmPassword
            }));
            setStep(2);
        }
    });

    const formikStep2 = useFormik({
      initialValues: initialValuesStep2,
      validationSchema: validationSchemaStep2,
      enableReinitialize: true,
      onSubmit: async (values) => {
          const formData = new FormData();
          formData.append('Email', values.email);
          formData.append('UserName', values.username);
          formData.append('Password', values.password);
          formData.append('Role', values.role);
          formData.append('FirstName', values.firstName);
          formData.append('LastName', values.lastName);
          formData.append('DateOfBirth', new Date(values.dateOfBirth).toISOString());
          formData.append('Bio', values.bio || '');
          if (values.image) {
              formData.append('ProfilePicture', values.image);
          }
  
          for (const pair of formData.entries()) {
              console.log(`${pair[0]}: ${pair[1]}`);
          }
  
          try {
            const response = await registerUser(formData);
            console.log(response.data);
        } catch (error) {
            console.error(error);
        }
      }
  });

    return (
        <>
            <Helmet>
                <title>Signup</title>
            </Helmet>
            <div className="signup">
                <div className="signupWrapper">
                    <div className="signupLeft">
                        <h3 className="signupLogo">Let's Make it Happen Together!</h3>
                    </div>
                    <div className="signupRight">
                        <h2>Create An Account</h2>
                        {step === 1 && (
                            <form onSubmit={formikStep1.handleSubmit} className="signupForm">
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="Email"
                                        variant="outlined"
                                        type="email"
                                        placeholder="Email"
                                        className={`signupInput ${formikStep1.touched.email && formikStep1.errors.email ? "errorInput" : ""}`}
                                        {...formikStep1.getFieldProps('email')}
                                        error={formikStep1.touched.email && Boolean(formikStep1.errors.email)}
                                        helperText={formikStep1.touched.email && formikStep1.errors.email}
                                    />
                                </div>
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="Username"
                                        variant="outlined"
                                        type="text"
                                        placeholder="Username"
                                        className={`signupInput ${formikStep1.touched.username && formikStep1.errors.username ? "errorInput" : ""}`}
                                        {...formikStep1.getFieldProps('username')}
                                        error={formikStep1.touched.username && Boolean(formikStep1.errors.username)}
                                        helperText={formikStep1.touched.username && formikStep1.errors.username}
                                    />
                                </div>
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="Create Password"
                                        variant="outlined"
                                        type="password"
                                        placeholder="Create Password"
                                        className={`signupInput ${formikStep1.touched.password && formikStep1.errors.password ? "errorInput" : ""}`}
                                        {...formikStep1.getFieldProps('password')}
                                        error={formikStep1.touched.password && Boolean(formikStep1.errors.password)}
                                        helperText={formikStep1.touched.password && formikStep1.errors.password}
                                    />
                                </div>
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="Confirm Password"
                                        variant="outlined"
                                        type="password"
                                        placeholder="Confirm Password"
                                        className={`signupInput ${formikStep1.touched.confirmPassword && formikStep1.errors.confirmPassword ? "errorInput" : ""}`}
                                        {...formikStep1.getFieldProps('confirmPassword')}
                                        error={formikStep1.touched.confirmPassword && Boolean(formikStep1.errors.confirmPassword)}
                                        helperText={formikStep1.touched.confirmPassword && formikStep1.errors.confirmPassword}
                                    />
                                </div>
                                <Button type="submit" variant="contained" className="signupButton">Next</Button>
                                <p className="signupLoginPrompt">
                                    Already have an account? <Link to="/login" className="signupLoginLink">Login</Link>
                                </p>
                            </form>
                        )}
                        {step === 2 && (
                            <form onSubmit={formikStep2.handleSubmit} className="signupForm">
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="First Name"
                                        variant="outlined"
                                        type="text"
                                        placeholder="First Name"
                                        className={`signupInput ${formikStep2.touched.firstName && formikStep2.errors.firstName ? "errorInput" : ""}`}
                                        {...formikStep2.getFieldProps('firstName')}
                                        error={formikStep2.touched.firstName && Boolean(formikStep2.errors.firstName)}
                                        helperText={formikStep2.touched.firstName && formikStep2.errors.firstName}
                                    />
                                </div>
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="Last Name"
                                        variant="outlined"
                                        type="text"
                                        placeholder="Last Name"
                                        className={`signupInput ${formikStep2.touched.lastName && formikStep2.errors.lastName ? "errorInput" : ""}`}
                                        {...formikStep2.getFieldProps('lastName')}
                                        error={formikStep2.touched.lastName && Boolean(formikStep2.errors.lastName)}
                                        helperText={formikStep2.touched.lastName && formikStep2.errors.lastName}
                                    />
                                </div>
                                <div className="formGroupRow">
                                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                                        <DatePicker
                                            format="DD-MM-YYYY"
                                            label="Date of birth"
                                            onChange={(value) => formikStep2.setFieldValue('dateOfBirth', value)}
                                            slotProps={{
                                                textField: {
                                                    variant: "outlined",
                                                    className: 'signupInput',
                                                    error: formikStep2.touched.dateOfBirth && Boolean(formikStep2.errors.dateOfBirth),
                                                    helperText: formikStep2.touched.dateOfBirth && formikStep2.errors.dateOfBirth,
                                                }
                                            }}
                                        />
                                    </LocalizationProvider>
                                    <FormControl
                                        variant="outlined"
                                        className={`signupInput ${formikStep2.touched.role && formikStep2.errors.role ? "errorInput" : ""}`}
                                        error={formikStep2.touched.role && Boolean(formikStep2.errors.role)}
                                    >
                                        <InputLabel id="role-label">Role</InputLabel>
                                        <Select
                                            labelId="role-label"
                                            id="role"
                                            value={formikStep2.values.role}
                                            onChange={(event) => formikStep2.setFieldValue('role', event.target.value)}
                                            label="Role"
                                        >
                                            <MenuItem value=""><em>Select role</em></MenuItem>
                                            <MenuItem value="admin">Admin</MenuItem>
                                            <MenuItem value="user">User</MenuItem>
                                        </Select>
                                        {formikStep2.touched.role && formikStep2.errors.role && (
                                            <FormHelperText>{formikStep2.errors.role}</FormHelperText>
                                        )}
                                    </FormControl>
                                </div>
                                <div className="formGroup">
                                    <TextField
                                        id="outlined-basic"
                                        label="Bio (optional)"
                                        variant="outlined"
                                        multiline
                                        rows={2}
                                        placeholder="Bio (optional)"
                                        className={`signupInput ${formikStep2.touched.bio && formikStep2.errors.bio ? "errorInput" : ""}`}
                                        {...formikStep2.getFieldProps('bio')}
                                        error={formikStep2.touched.bio && Boolean(formikStep2.errors.bio)}
                                        helperText={formikStep2.touched.bio && formikStep2.errors.bio}
                                    />
                                </div>
                                <div className="formGroup">
                                    <input
                                        type="file"
                                        className="signupInput"
                                        onChange={(event) => {
                                            if (event.currentTarget.files && event.currentTarget.files[0]) {
                                                formikStep2.setFieldValue("image", event.currentTarget.files[0]);
                                            }
                                        }}
                                    />
                                </div>
                                <Button type="submit" variant="contained" className="signupButton">Sign Up</Button>
                                <p className="signupLoginPrompt">
                                    Already have an account? <Link to="/login" className="signupLoginLink">Login</Link>
                                </p>
                            </form>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
};

export default Signup;
