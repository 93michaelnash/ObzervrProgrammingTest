import React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as CounterStore from '../store/Counter';
import * as mapboxgl from 'mapbox-gl';
import * as csv2geojson from 'csv2geojson';
import * as d3 from "d3";

mapboxgl.accessToken = 'pk.eyJ1IjoibWljaGFlbG5hc2g5MyIsImEiOiJja2J4MnB6ZmgwM3RoMnNwam1paTF3aWhwIn0.HCrWrLGpq52BRw73XudxEw';

class Map extends React.Component {
  mapContainerRef = React.createRef();

  state = {
    lng: 5,
    lat: 34,
    zoom: 2,
  };

  componentDidMount() {
    d3.csv("https://storage.cloud.google.com/obzervr-taxi-bucket/1_2015-000000000000.csv").then(function (csvData) {
      csv2geojson.csv2geojson(
        csvData,
        {
          latfield: "dropoff_latitude",
          lonfield: "dropoff_longitude",
          delimiter: ",",
        },
        function (err, data) {
          debugger;
        }
      );
    });

    const map = new mapboxgl.Map({
      container: this.mapContainerRef.current,
      style: "mapbox://styles/mapbox/streets-v11",
      center: [this.state.lng, this.state.lat],
      zoom: this.state.zoom,
    });

    map.on("move", () => {
      this.setState({
        lng: map.getCenter().lng.toFixed(4),
        lat: map.getCenter().lat.toFixed(4),
        zoom: map.getZoom().toFixed(2),
      });
    });
  }

  render() {
    return (
      <div>
        <div ref={this.mapContainerRef} className="mapContainer" />
      </div>
    );
  }
};

export default connect()(Map);
