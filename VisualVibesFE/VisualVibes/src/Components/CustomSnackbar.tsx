import { Alert, Snackbar } from '@mui/material';
import React from 'react'

interface CustomSnackbarInterface {
  isOpen: boolean;
  setIsOpen: (value: boolean) => void;
  hideDuration: number;
  snackbarMessage: string;
  snackbarSeverity: 'success' | 'error';
}

const CustomSnackbar: React.FC<CustomSnackbarInterface> = ({ isOpen, setIsOpen, hideDuration, snackbarMessage, snackbarSeverity }) => {

  return (
    <Snackbar
        open={isOpen}
        autoHideDuration={hideDuration}
        onClose={() => setIsOpen(false)}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
    >
        <Alert onClose={() => setIsOpen(false)} severity={snackbarSeverity}>
            {snackbarMessage}
        </Alert>
    </Snackbar>
  )
}

export default CustomSnackbar;