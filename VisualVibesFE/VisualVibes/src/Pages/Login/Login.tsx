import React from 'react';
import './Login.css';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';
import * as Yup from 'yup';
import { loginUser } from '../../Services/AuthenticationServiceApi';
import { UserLoginModel } from '../../Models/UserLoginModel';
import { useFormik } from 'formik';

const Login: React.FC = () => {
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
                navigate('/');
            } catch (error) {
                console.error('Login failed:', error);
                alert('Login failed. Please check your credentials and try again.');
            }
        }
    });

    const navigate = useNavigate();

    const handleSignup = () => {
        navigate('/signup');
    };

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
                        <div className="loginBox">
                            <form onSubmit={formik.handleSubmit} className="loginForm">
                            <label className={`loginLabel ${formik.touched.email && formik.errors.email ? "errorLabel" : ""}`}>
                                    { formik.touched.email && formik.errors.email ? formik.errors.email : "Email"}
                                </label>
                                <input
                                    type="email"
                                    name="email"
                                    value={formik.values.email}
                                    onChange={formik.handleChange}
                                    placeholder="Email Address"
                                    className="loginInput"
                                    onBlur={formik.handleBlur}
                                />
                                <label className={`loginLabel ${formik.touched.password && formik.errors.password ? "errorLabel" : ""}`}>
                                    { formik.touched.password && formik.errors.password ? formik.errors.password : "Password"}
                                </label>
                                <input
                                    type="password"
                                    name="password"
                                    value={formik.values.password}
                                    onChange={formik.handleChange}
                                    placeholder="Password"
                                    className="loginInput"
                                    onBlur={formik.handleBlur}
                                />
                                <button type="submit" className="loginButton">Log In</button>
                                <button type="button" className="loginRegistrationButton" onClick={handleSignup}>
                                    Don't have an account? Sign up
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default Login;
