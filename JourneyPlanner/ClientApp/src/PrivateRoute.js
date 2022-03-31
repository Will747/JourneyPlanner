import React, { useEffect } from "react";

import { useSelector } from "react-redux";
import { selectUser } from "./reducers/user";

import CircularProgress from '@material-ui/core/CircularProgress';

import { useHistory } from "react-router-dom";

function PrivateRoute(props)
{
    const user = useSelector(selectUser);
    let history = useHistory();

    useEffect(() => {
        if (user === undefined || user.username === null)
        {
            history.push('/login');
        }
    }, [user, history])

    if (user === undefined || user.username === null)
    {
        return <CircularProgress/>
    } else {
        return props.children
    }
}

export default PrivateRoute;
