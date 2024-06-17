import EmailRoundedIcon from "@mui/icons-material/EmailRounded";
import LockRoundedIcon from "@mui/icons-material/LockRounded";

export const formLabelEmail = () => {
  return (
    <div style={{ display: "flex", alignItems: "center" }}>
      <EmailRoundedIcon style={{ marginRight: 5 }} />
      Email Address
    </div>
  );
};

export const formLabelPassword = () => {
  return (
    <div style={{ display: "flex", alignItems: "center" }}>
      <LockRoundedIcon style={{ marginRight: 5 }} />
      Password
    </div>
  );
};
