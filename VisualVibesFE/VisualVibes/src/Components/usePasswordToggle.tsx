import { useState } from 'react';
import { IconButton, InputAdornment, TextFieldProps } from '@mui/material';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';

const usePasswordToggle = (): { InputProps: TextFieldProps['InputProps'] } => {
    const [visible, setVisible] = useState<boolean>(false);

    const handleClick = () => {
        setVisible((prev) => !prev);
    };

    const InputProps: TextFieldProps['InputProps'] = {
        type: visible ? 'text' : 'password',
        endAdornment: (
            <InputAdornment position="end">
                <IconButton
                    aria-label={visible ? "hide password" : "show password"}
                    onClick={handleClick}
                    edge="end"
                >
                    {visible ? <Visibility /> : <VisibilityOff />}
                </IconButton>
            </InputAdornment>
        )
    };

    return { InputProps };
};

export default usePasswordToggle;
