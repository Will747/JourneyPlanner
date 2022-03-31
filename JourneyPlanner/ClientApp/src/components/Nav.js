import React from "react";
import { Link } from "react-router-dom";

import axios from "axios";

import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  FormControlLabel,
  Switch,
  Tooltip
} from "@material-ui/core";

import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import AccountBoxIcon from '@material-ui/icons/AccountBox';

import { useSelector, useDispatch } from "react-redux";
import { selectUser, OverwriteUserData } from "../reducers/user";

import classes from "./Nav.module.css";

function Nav({ darkMode, handleDark }) {
  const user = useSelector(selectUser);
  const dispatch = useDispatch();

  const handleLogout = () => {
    axios.get("/api/v1/user/logout")
    .then(
        dispatch(OverwriteUserData(undefined))
    );
  }

  //https://material-ui.com/components/app-bar/
  return (
    <AppBar color={darkMode ? "default" : "primary"} position="static">
      <Toolbar>
        <Typography component={Link} to="/" variant="h6" className={classes.left}>
          Journey Planner
        </Typography>
        <div>
          <FormControlLabel
            control={<Switch checked={darkMode} onChange={handleDark} />}
            label="Dark Mode"
          />
          { user === undefined || user.username === null ?
          <Button component={Link} to="/login" color="inherit">Login</Button>
          :
          <>
          <Tooltip title="Account Settings">
            <Button component={Link} to="/account" color="inherit"><AccountBoxIcon/></Button>
          </Tooltip>
          <Tooltip title="Logout">
            <Button onClick={handleLogout} color="inherit"><ExitToAppIcon/></Button>
          </Tooltip>
          </>
          }

          <Button component={Link} to="/map" color="inherit">View Map</Button>
        </div>
      </Toolbar>
    </AppBar>
  );
}

export default Nav;
