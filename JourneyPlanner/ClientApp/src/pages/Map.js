import React, {useEffect, useState} from 'react';
import axios from 'axios';

import PageContainer from "../components/PageContainer";
import MapViewer from "../components/MapViewer";

function Map() {
    const [stations, setStations] = useState([]);
    const [lines, SetLines] = useState([]);

    useEffect(() => {
        axios.get("/api/v1/stations").then((response) => {
            setStations(response.data);
        });

        axios.get("/api/v1/stations/lines").then((response) => {
            SetLines(response.data);
        });
    }, []);

    return ( 
        <PageContainer >
            <MapViewer zoom={5} center={{ lat: 54.2808, lng: -5.4051 }} stations={stations} lines={lines} />
        </PageContainer>
    );
}

export default Map;
