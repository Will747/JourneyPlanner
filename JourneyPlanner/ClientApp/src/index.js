import React from "react";
import reactDOM from "react-dom";
import { Provider } from 'react-redux';
import "./index.css";

import store from './store';

import App from "./App";

function Root() {
  return (
    <React.StrictMode>
        <Provider store={store}>
            <App />
        </Provider>
    </React.StrictMode>
  );
}

reactDOM.render(<Root />, document.getElementById("root"));
