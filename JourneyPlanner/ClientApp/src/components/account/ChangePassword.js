import React, {useState} from "react";

import axios from "axios";

import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Box from '@material-ui/core/Box';

function ChangePassword({ open, handleClose })
{
    const [oldPw, setOldPw] = useState("");
    const [newPw, setNewPw] = useState("");
    const [newPw1, setNewPw1] = useState("");
    const [newPwMatch, setNewPwMatch] = useState(true);

    const handleNewPw = event => {
        setNewPw(event.target.value);

        if (newPw1 == event.target.value)
        {
            setNewPwMatch(true);
        } else if (newPw1.Length > 0) {
            setNewPwMatch(false);
        }
    }

    const handleNewPw1 = event => {
        setNewPw1(event.target.value);

        if (event.target.value == newPw)
        {
            setNewPwMatch(true);
        } else {
            setNewPwMatch(false);
        }
    }

    const handleChange = () => {
        axios.post('/api/v1/user/changePw', [ oldPw, newPw ]).then(
            handleClose
        );
    }

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Change Password</DialogTitle>
            <DialogContent>
            <Box m={2}>
                <TextField
                    label="Current Password"
                    type="password"
                    variant="outlined"
                    onChange={event => setOldPw(event.target.value)}
                    value={oldPw}
                    />
                    </Box><Box m={2}>
                  <TextField
                    error={!newPwMatch}
                    label="New Password"
                    type="password"
                    variant="outlined"
                    onChange={handleNewPw}
                    value={newPw}
                    />
                    </Box><Box m={2}>
                    <TextField
                    error={!newPwMatch}
                    helperText={!newPwMatch ? "Passwords don't match" : ""}
                    label="Repeat New Password"
                    type="password"
                    variant="outlined"
                    onChange={handleNewPw1}
                    value={newPw1}
                    />  
                    </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose} color="primary">Cancel</Button>
                <Button onClick={handleChange} color="primary">Change</Button>
            </DialogActions>
        </Dialog>
    );
}

export default ChangePassword;
