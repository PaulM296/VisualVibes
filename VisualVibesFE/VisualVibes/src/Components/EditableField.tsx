import React, { ChangeEvent } from 'react';
import { TextField } from '@mui/material';

interface EditableFieldProps {
    label: string;
    value: string;
    editable: boolean;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
}

const EditableField: React.FC<EditableFieldProps> = ({ label, value, editable, onChange }) => {
    return (
        <TextField
            label={label}
            value={value}
            onChange={onChange}
            fullWidth
            margin="normal"
            variant="outlined"
            InputProps={{
                readOnly: !editable,
            }}
        />
    );
};

export default EditableField;
