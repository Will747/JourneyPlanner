import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import axios from "axios";

import { useDispatch } from "react-redux";
import checkLogin from "../checkLogin";

import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Snackbar from "@material-ui/core/Snackbar";
import MuiAlert from "@material-ui/lab/Alert";
import Box from '@material-ui/core/Box';
import Container from '@material-ui/core/Container';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';

function NewUser() {
  let history = useHistory();
  const dispatch = useDispatch();
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [failed, setFailed] = useState(false);
  const [alert, setAlert] = useState(false);

  const handleBack = () => {
    history.goBack();
  };

  const handleCreate = () => {
    if (password.length > 6)
    {
        axios.post("/api/v1/login/create", {
            Username: username,
            Email: email,
            Password: password,
          }).then((response) => {
              checkLogin(dispatch);
              history.push('/');
          });
    } else {
        setFailed(true);
        setAlert(true);
    }
  };

  const handleCloseWarning = () => {
      setAlert(false)
  }

  return (
    <Container maxWidth="sm">
        <Box m={2}>
        <Snackbar
          open={alert}
          autoHideDuration={10000}
          onClose={handleCloseWarning}
        >
          <MuiAlert severity="error" variant="filled">
            Enter a longer password! (Must be at least 7 characters)
          </MuiAlert>
        </Snackbar>
        <Card variant="outlined">
        <CardContent>
        <Typography variant="h5">Create Account</Typography>
        <Box p={2}>
          <Button onClick={handleBack} variant="outlined">
            Back
          </Button>
          </Box>
          <Box p={2}>
          <TextField
            error={failed}
            label="Username"
            value={username}
            onChange={(event) => setUsername(event.target.value)}
            variant="outlined"
          />
          </Box>
          <Box p={2}>
          <TextField
            error={failed}
            label="Email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            variant="outlined"
          />
          </Box>
          <Box p={2}>
          <TextField
            error={failed}
            type="password"
            label="Password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            variant="outlined"
          />
          </Box>
          </CardContent>
            <CardActions>
                <Button onClick={handleCreate} variant="contained" color="primary">
                Create Account
                </Button>
            </CardActions>
      </Card>
      </Box>
      </Container>
  );
}

export default NewUser;
