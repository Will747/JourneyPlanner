import React, {useState} from "react";
import { useDispatch } from "react-redux";
import axios from "axios";

import classes from "../../pages/Account.module.css";

import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Snackbar from "@material-ui/core/Snackbar";
import MuiAlert from "@material-ui/lab/Alert";

import checkLogin from "../../checkLogin";

function DeleteAccount({ open, handleClose })
{
    const dispatch = useDispatch();
    const [password, setPassword] = useState("");
    const [alert, setAlert] = useState(false);

    const handleDelete = () => {
        axios.post('/api/v1/user/delete', {password})
        .then(response => {
            if (response.data) {
                checkLogin(dispatch);
                handleClose();
            } else {
                setAlert(true);
            }
        }
        );
    }

    return (
        <>
        <Snackbar
          open={alert}
          autoHideDuration={10000}
          onClose={() => setAlert(false)}
            >
                <MuiAlert severity="error" variant="filled">
                Error: Failed to delete account!
            </MuiAlert>
            </Snackbar>
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Delete Account</DialogTitle>
            <DialogContent>
                <TextField
                    label="Account Password"
                    type="password"
                    variant="outlined"
                    onChange={event => setPassword(event.target.value)}
                    value={password}
                    />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose} color="primary">Cancel</Button>
                <Button onClick={handleDelete} variant="contained" className={classes.deleteButton} color="primary">Delete</Button>
            </DialogActions>
        </Dialog>
        </>
    );
}

export default DeleteAccount;
