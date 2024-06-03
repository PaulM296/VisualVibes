import React, { useState } from 'react';
import './Signup.css';
import { Helmet } from 'react-helmet-async';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import { registerUser } from '../../Services/AuthenticationServiceApi';

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
        role: 'user',
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

            // Log the FormData entries
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
                                    <label className={`signupLabel ${formikStep1.touched.email && formikStep1.errors.email && "errorLabel"}`}>
                                        {formikStep1.touched.email && formikStep1.errors.email ? formikStep1.errors.email : "Email"}
                                    </label>
                                    <input
                                        type="email"
                                        placeholder="Email"
                                        className="signupInput"
                                        {...formikStep1.getFieldProps('email')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep1.touched.username && formikStep1.errors.username && "errorLabel"}`}>
                                        {formikStep1.touched.username && formikStep1.errors.username ? formikStep1.errors.username : "Username"}
                                    </label>
                                    <input
                                        type="text"
                                        placeholder="Username"
                                        className="signupInput"
                                        {...formikStep1.getFieldProps('username')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep1.touched.password && formikStep1.errors.password && "errorLabel"}`}>
                                        {formikStep1.touched.password && formikStep1.errors.password ? formikStep1.errors.password : "Password"}
                                    </label>
                                    <input
                                        type="password"
                                        placeholder="Create Password"
                                        className="signupInput"
                                        {...formikStep1.getFieldProps('password')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep1.touched.confirmPassword && formikStep1.errors.confirmPassword && "errorLabel"}`}>
                                        {formikStep1.touched.confirmPassword && formikStep1.errors.confirmPassword ? formikStep1.errors.confirmPassword : "Confirm Password"}
                                    </label>
                                    <input
                                        type="password"
                                        placeholder="Confirm Password"
                                        className="signupInput"
                                        {...formikStep1.getFieldProps('confirmPassword')}
                                    />
                                </div>
                                <button type="submit" className="signupButton">Next</button>
                            </form>
                        )}
                        {step === 2 && (
                            <form onSubmit={formikStep2.handleSubmit} className="signupForm">
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep2.touched.firstName && formikStep2.errors.firstName && "errorLabel"}`}>
                                        {formikStep2.touched.firstName && formikStep2.errors.firstName ? formikStep2.errors.firstName : "First Name"}
                                    </label>
                                    <input
                                        type="text"
                                        placeholder="First Name"
                                        className="signupInput"
                                        {...formikStep2.getFieldProps('firstName')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep2.touched.lastName && formikStep2.errors.lastName && "errorLabel"}`}>
                                        {formikStep2.touched.lastName && formikStep2.errors.lastName ? formikStep2.errors.lastName : "Last Name"}
                                    </label>
                                    <input
                                        type="text"
                                        placeholder="Last Name"
                                        className="signupInput"
                                        {...formikStep2.getFieldProps('lastName')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep2.touched.dateOfBirth && formikStep2.errors.dateOfBirth && "errorLabel"}`}>
                                        {formikStep2.touched.dateOfBirth && formikStep2.errors.dateOfBirth ? formikStep2.errors.dateOfBirth : "Date of Birth"}
                                    </label>
                                    <input
                                        type="date"
                                        className="signupInput"
                                        {...formikStep2.getFieldProps('dateOfBirth')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className={`signupLabel ${formikStep2.touched.role && formikStep2.errors.role && "errorLabel"}`}>
                                        {formikStep2.touched.role && formikStep2.errors.role ? formikStep2.errors.role : "Role"}
                                    </label>
                                    <select className="signupInput" {...formikStep2.getFieldProps('role')}>
                                        <option value="">Select role</option>
                                        <option value="admin">Admin</option>
                                        <option value="user">User</option>
                                    </select>
                                </div>
                                <div className="formGroup">
                                    <label className="signupLabel">Bio (optional)</label>
                                    <textarea
                                        placeholder="Bio (optional)"
                                        className="signupInput"
                                        {...formikStep2.getFieldProps('bio')}
                                    />
                                </div>
                                <div className="formGroup">
                                    <label className="signupLabel">Profile Image</label>
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
                                <button type="submit" className="signupButton">Sign Up</button>
                            </form>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
};

export default Signup;