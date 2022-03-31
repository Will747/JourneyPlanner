import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useHistory } from "react-router-dom";
import classes from "./routes/RoutesBox.module.css";

import Accordion from "@material-ui/core/Accordion";
import AccordionSummary from "@material-ui/core/AccordionSummary";
import AccordionDetails from "@material-ui/core/AccordionDetails";
import Button from '@material-ui/core/Button';
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";

import axios from "axios";

import { overwriteUserInput } from "../reducers/route";
import { selectRoutes, OverwriteRoutes, RemoveRoute } from "../reducers/user";

import Tile from "./Tile";

function SavedJourneys()
{
    const dispatch = useDispatch();
    const history = useHistory();
    const routes = useSelector(selectRoutes);

    useEffect(() => {
        axios.get('/api/v1/user/routes').then(
            response => dispatch(OverwriteRoutes(response.data))
        )
    }, [dispatch])

    const handleOpen = (route) => {
        let type;
        switch(route.type)
        {
            case 1:
                type = "Single"
                break;
            case 2:
                type = "Return"
                break;
            case 3:
                type = "Multi-Stop"
                break;
        }

        let algorithm;
        switch(route.algorithmType)
        {
            case 1:
                algorithm = "Distance"
                break;
            case 2:
                algorithm = "Timetable"
                break;
        }

        dispatch(overwriteUserInput({
            stops: route.stops,
            type: type,
            algorithm: algorithm
        }))

        history.push('/route');
    }

    const handleDelete = (route, index) => {
        axios.delete('/api/v1/user/route/' + route.id);
        dispatch(RemoveRoute(index))
    }

    return (
        <Tile>
            {
                routes.length === 0 ?
                <h4>No Saved Journeys</h4> :
                (
                    <>
                      <h4>Saved Journeys:</h4>
                      <div className={classes.List}>
                        {routes.map((route, index) => (
                          <Accordion key={'route' + index}>
                            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                              {route.stops[0].name + ' to ' + route.stops[route.stops.length - 1].name}
                            </AccordionSummary>
                            <AccordionDetails>
                              <Button onClick={() => handleOpen(route)} variant="contained" color="primary">Open</Button>
                              <Button variant="contained" color="secondary">Edit</Button>
                              <Button onClick={() => handleDelete(route, index)} variant="contained">Delete</Button>
                            </AccordionDetails>
                          </Accordion>
                        ))}
                      </div>
                    </>
                  )
            }
        </Tile>
    );
}

export default SavedJourneys;
