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

function Login() {
  let history = useHistory();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loginFailed, setLoginFailed] = useState(false);
  const [alert, setAlert] = useState(false);
  const dispatch = useDispatch();

  const handleBack = () => {
    history.goBack();
  };

  const handleCreateAccount = () => {
    history.push('/create_user')
  }

  const handleLogin = event => {
    axios.post("/api/v1/login", {
      username: username,
      password: password,
    }).then((response) => {
      if (response.data === true) {
        //If the login is valid, gets user data
        checkLogin(dispatch);
        history.push('/');
      } else {
        setLoginFailed(true);
        setAlert(true);
      }
    });
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
            Incorrect Username or Password!
          </MuiAlert>
        </Snackbar>
        <Card variant="outlined">
        <CardContent>
        <Typography variant="h5">Login</Typography>
        <Box p={2}>
          <Button onClick={handleBack} variant="outlined">
            Back
          </Button>
          </Box>
          <Box p={2}>
          <TextField
            error={loginFailed}
            label="Username"
            value={username}
            onChange={(event) => setUsername(event.target.value)}
            variant="outlined"
          />
          </Box><Box p={2}>
          <TextField
            error={loginFailed}
            type="password"
            label="Password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            variant="outlined"
          />
          </Box>
          </CardContent>
                    <CardActions>
                    <Button onClick={handleLogin} variant="contained" color="primary">
              Login
            </Button>
            <Button onClick={handleCreateAccount} variant="contained">Create Account</Button>
                    </CardActions>
      </Card>
      </Box>
      </Container>
  );
}

export default Login;
