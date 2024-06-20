import { Helmet } from "react-helmet-async";
import Feed from "../../Components/Feed/Feed";
import Navbar from "../../Components/Navbar/Navbar";
import Rightbar from "../../Components/Rightbar/Rightbar";
import Sidebar from "../../Components/Sidebar/Sidebar";
import "./Home.css";
import { useEffect, useState } from "react";
import { Alert, Snackbar } from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";

const Home: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");
  const [snackbarSeverity, setSnackbarSeverity] = useState<"success" | "error">(
    "success"
  );

  useEffect(() => {
    if (location.state && location.state.message) {
      setSnackbarMessage(location.state.message);
      setSnackbarSeverity(location.state.severity || "success");
      setSnackbarOpen(true);

      navigate(location.pathname, { replace: true });

      const timer = setTimeout(() => {
        setSnackbarOpen(false);
      }, 3000);
      return () => clearTimeout(timer);
    }
  }, [location.state, navigate, location.pathname]);

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  return (
    <div className="home">
      <Helmet>
        <title>Homepage</title>
      </Helmet>
      <div className="homeContainer">
        <Navbar />
        <div className="homepageContent">
          <div className="stickySidebar">
            <Sidebar />
          </div>
          <Feed />
          <div className="stickyRightBar">
            <Rightbar />
          </div>
        </div>
      </div>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={3000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert onClose={handleCloseSnackbar} severity={snackbarSeverity}>
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </div>
  );
};

export default Home;
