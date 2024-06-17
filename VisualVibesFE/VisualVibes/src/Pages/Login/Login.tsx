import React, { useState } from "react";
import "./Login.css";
import { Helmet } from "react-helmet-async";
import { Link, useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { loginUser } from "../../Services/AuthenticationServiceApi";
import { UserLoginModel } from "../../Models/UserLoginModel";
import { useFormik } from "formik";
import { Button, TextField } from "@mui/material";
import EmailRoundedIcon from "@mui/icons-material/EmailRounded";
import LockRoundedIcon from "@mui/icons-material/LockRounded";
import usePasswordToggle from "../../Hooks/usePasswordToggle";
import CustomSnackbar from "../../Components/CustomSnackbar";

const Login: React.FC = () => {
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");
  const [snackbarSeverity, setSnackbarSeverity] = useState<"success" | "error">(
    "success"
  );
  const navigate = useNavigate();
  const passwordInputProps = usePasswordToggle();

  const formLabelEmail = () => {
    return (
      <div style={{ display: "flex", alignItems: "center" }}>
        <EmailRoundedIcon style={{ marginRight: 5 }} />
        Email Address
      </div>
    );
  };

  const formLabel = () => {
    return (
      <div style={{ display: "flex", alignItems: "center" }}>
        <LockRoundedIcon style={{ marginRight: 5 }} />
        Password
      </div>
    );
  };

  const formik = useFormik<UserLoginModel>({
    initialValues: {
      email: "",
      password: "",
    },

    validationSchema: Yup.object({
      email: Yup.string().email("Invalid email").required("Email is required"),
      password: Yup.string()
        .min(8, "Password must be at least 8 characters")
        .required("Password is required"),
    }),

    onSubmit: async (values) => {
      try {
        const response = await loginUser(values);
        localStorage.setItem("token", response.data.token);
        setSnackbarSeverity("success");
        setSnackbarMessage("Login successful");
        setSnackbarOpen(true);
        navigate("/", {
          state: { message: "Login successful", severity: "success" },
        });
      } catch (error) {
        console.error("Login failed:", error);
        setSnackbarSeverity("error");
        setSnackbarMessage(
          "Login failed. Please check your credentials and try again."
        );
        setSnackbarOpen(true);
      }
    },
  });

  return (
    <>
      <Helmet>
        <title>Login</title>
      </Helmet>
      <div className="loginContainer">
        <div className="loginWrapper">
          <div className="loginContentLeft">
            <h3 className="loginLogo">VisualVibes</h3>
            <span className="loginDescription">Connect with your friends</span>
          </div>
          <div className="loginContentRight">
            <h2>Log In</h2>
            <div className="loginFormContainer">
              <form onSubmit={formik.handleSubmit} className="loginForm">
                <TextField
                  id="outlined-email"
                  label={formLabelEmail()}
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
                  id="outlined-password"
                  label={formLabel()}
                  variant="outlined"
                  name="password"
                  placeholder="Password"
                  value={formik.values.password}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  error={
                    formik.touched.password && Boolean(formik.errors.password)
                  }
                  helperText={formik.touched.password && formik.errors.password}
                  {...passwordInputProps}
                />
                <Button
                  type="submit"
                  variant="contained"
                  className="loginButton"
                >
                  Log in
                </Button>
                  <p className="loginRegistration">
                    {"Don't have an account? " }
                    <Link to="/signup" className="signupLink">
                      {" Signup now"}
                    </Link>
                  </p>
              </form>
            </div>
          </div>
        </div>
      </div>
      <CustomSnackbar
        isOpen={snackbarOpen}
        setIsOpen={setSnackbarOpen}
        hideDuration={3000}
        snackbarMessage={snackbarMessage}
        snackbarSeverity={snackbarSeverity}
      />
    </>
  );
};

export default Login;
