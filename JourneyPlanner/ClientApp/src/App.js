import React, { useState, useEffect } from "react";

//https://reactrouter.com/web/guides/quick-start
import { BrowserRouter, Switch as RouterSwitch, Route } from "react-router-dom";

import CssBaseline from '@material-ui/core/CssBaseline';
import { createMuiTheme } from "@material-ui/core";
import { ThemeProvider } from "@material-ui/core/styles";

import Nav from "./components/Nav";
import Login from "./pages/Login";
import FrontPage from "./pages/Front";
import PathView from "./pages/PathView";
import Map from "./pages/Map";
import Account from "./pages/Account";

import PrivateRoute from "./PrivateRoute";

import { useDispatch } from "react-redux";
import checkLogin from "./checkLogin";
import NewUser from "./pages/NewUser";

function App() {
  const [darkMode, setDarkMode] = useState(false);
  const dispatch = useDispatch();

  useEffect(() => {  
    checkLogin(dispatch);
  }, [dispatch])

  const theme = createMuiTheme({
    palette: {
      primary: {
        light: "#BBDEFB",
        main: "#2196F3",
        dark: "#1976D2",
      },
      secondary: {
        main: "#CDDC39",
      },
    },
  });

  const darkTheme = createMuiTheme({
    palette: {
      type: "dark",
    },
  });

  const handleDarkModeChange = () => {
    setDarkMode((darkMode) => !darkMode);
  };

  return (
      <ThemeProvider theme={darkMode ? darkTheme : theme}>
        <CssBaseline>
        <BrowserRouter>
          <Nav darkMode={darkMode} handleDark={handleDarkModeChange} />
          <RouterSwitch>
            <Route path="/" exact>
              <FrontPage />
            </Route>
            <Route path="/login">
              <Login />
            </Route>
            <Route path="/create_user">
              <NewUser/>
            </Route>
            <Route path="/route">
              <PathView />
            </Route>
            <Route path="/map">
              <Map/>
            </Route>
            <Route path="/account">
              <PrivateRoute>
                <Account/>
              </PrivateRoute>
            </Route>
          </RouterSwitch>
        </BrowserRouter>
          <div className="credit-box">
            <p>Station locations sourced from the <a href="https://transportapi.com">TransportAPI</a></p>
            <p>Timetable data from <a href="https://realtimetrains.co.uk/">Realtime Trains</a></p>
          </div>
        </CssBaseline>
      </ThemeProvider>
  );
}

export default App;
