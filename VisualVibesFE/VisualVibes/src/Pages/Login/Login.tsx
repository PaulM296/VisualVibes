import React, { useState } from 'react';
import './Login.css';
import { Helmet } from 'react-helmet-async';
import { Link, useNavigate } from 'react-router-dom';
import * as Yup from 'yup';
import { loginUser } from '../../Services/AuthenticationServiceApi';
import { UserLoginModel } from '../../Models/UserLoginModel';
import { useFormik } from 'formik';
import { Alert, Button, Snackbar, TextField } from '@mui/material';

const Login: React.FC = () => {
    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMessage, setSnackbarMessage] = useState('');
    const [snackbarSeverity, setSnackbarSeverity] = useState<'success' | 'error'>('success');

    const handleCloseSnackbar = () => {
        setSnackbarOpen(false);
    };

    const formik = useFormik<UserLoginModel>({
        initialValues: {
            email: '',
            password: ''
        },

        validationSchema: Yup.object({
            email: Yup.string().email('Invalid email').required('Email is required'),
            password: Yup.string().min(8, 'Password must be at least 8 characters').required('Password is required')
        }),

        onSubmit: async (values) => {
            try {
                const response = await loginUser(values);
                console.log(response.data);
                localStorage.setItem('token', response.data.token);
                setSnackbarSeverity('success');
                setSnackbarMessage('Login successful');
                setSnackbarOpen(true);
                navigate('/', { state: { message: 'Login successful', severity: 'success' } });
            } catch (error) {
                console.error('Login failed:', error);
                setSnackbarSeverity('error');
                setSnackbarMessage('Login failed. Please check your credentials and try again.');
                setSnackbarOpen(true);
            }
        }
    });

    const navigate = useNavigate();

    return (
        <>
            <Helmet>
                <title>Login</title>
            </Helmet>
            <div className="login">
                <div className="loginWrapper">
                    <div className="loginLeft">
                        <h3 className="loginLogo">VisualVibes</h3>
                        <span className="loginDescription">
                            Connect with your friends
                        </span>
                    </div>
                    <div className="loginRight">
                        <h2>Log In</h2>
                        <div className="loginBox">
                            <form onSubmit={formik.handleSubmit} className="loginForm">
                                <TextField
                                    id="outlined-basic"
                                    label="Email Address"
                                    variant="outlined"
                                    name="email"
                                    placeholder="Email Address"
                                    value={formik.values.email}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    error={formik.touched.email && Boolean(formik.errors.email)}
                                    helperText={formik.touched.email && formik.errors.email}
                                />
                                <TextField
                                    id="outlined-basic"
                                    label="Password"
                                    variant="outlined"
                                    name="password"
                                    type="password"
                                    placeholder="Password"
                                    value={formik.values.password}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    error={formik.touched.password && Boolean(formik.errors.password)}
                                    helperText={formik.touched.password && formik.errors.password}
                                />
                                <Button type="submit" variant="contained" className="loginButton">Log in</Button>
                                <div className="registration">
                                    <p className="loginRegistration">Don't have an account? <Link to="/signup" className="signupLink">Signup now</Link></p>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
            <Snackbar
                open={snackbarOpen}
                autoHideDuration={3000}
                onClose={handleCloseSnackbar}
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                <Alert onClose={handleCloseSnackbar} severity={snackbarSeverity}>
                    {snackbarMessage}
                </Alert>
            </Snackbar>
        </>
    );
};

export default Login;
