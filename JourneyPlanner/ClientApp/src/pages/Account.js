import React, {useState, useEffect} from "react";
import { useSelector, useDispatch } from "react-redux";
import axios from "axios";

import classes from "./Account.module.css";

import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Box from '@material-ui/core/Box';

import { selectUser } from "../reducers/user";
import checkLogin from "../checkLogin";

import ChangePassword from "../components/account/ChangePassword";
import DeleteAccount from "../components/account/DeleteAccount";

function Account()
{
    const dispatch = useDispatch();
    const userData = useSelector(selectUser);
    const [username, setUsername] = useState();
    const [email, setEmail] = useState();
    const [changePw, setChangePw] = useState(false);
    const [deleteUser, setDeleteUser] = useState(false);

    useEffect(() => {
        setUsername(userData.username);
        setEmail(userData.email);
    }, [userData])

    const handleUpdateDetails = () => {
        axios.post('/api/v1/user/update', { Id: userData.id ,Username: username, Email: email}).then(
            checkLogin(dispatch)
        )
    }

    return (
        <Container maxWidth="sm">
            <Box m={2}>
                <ChangePassword
                    open={changePw}
                    handleClose={() => setChangePw(false)}
                />
                <DeleteAccount
                    open={deleteUser}
                    handleClose={() => setDeleteUser(false)}
                    />
                <Card variant="outlined">
                    <CardContent>
                        <Typography variant="h5">Update Account Details</Typography>
                        <Box p={2}>
                            <TextField 
                                variant="outlined" 
                                label="Username" 
                                value={username} 
                                onChange={event => setUsername(event.target.value)}>
                            </TextField>
                        </Box>
                        <Box p={2}>
                            <TextField 
                            variant="outlined" 
                            label="Email" 
                            value={email} 
                            onChange={event => setEmail(event.target.value)}>
                            </TextField>
                        </Box>
                    </CardContent>
                    <CardActions>
                        <Button variant="contained" color="primary" onClick={handleUpdateDetails}>Save</Button>
                    </CardActions>
                </Card>
            </Box>
            <Button variant="contained" color="secondary" onClick={() => setChangePw(true)}>Change Password</Button>
            <Button variant="contained" color="primary" onClick={() => setDeleteUser(true)} className={classes.deleteButton}>Delete Account</Button>
        </Container>
        );
}

export default Account;
