/* global google */

import React from "react";
import GoogleMapReact from "google-map-react";
import "../styles/site.css";
import { connect } from 'react-redux';
import * as TaxiDataStore from '../store/TaxiData';

class MapContainer extends React.Component {
  state = {
    heatMapData: [],
  };

  componentDidMount() {
    this.ensureDataFetched();
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.taxiData !== prevProps.taxiData) {
      if (this.props.taxiData.length > 0) {
        if (this._googleMap !== undefined) {
          this.state.heatMapData = this.props.taxiData
          .map((item) => {
            item = JSON.parse(item);
            return { location: new google.maps.LatLng(item.dropoff_latitude, item.dropoff_longitude), weight: 3 };
          })
          .concat(
            this.props.taxiData.map((item) => {
              item = JSON.parse(item);
              return { location: new google.maps.LatLng(item.pickup_latitude, item.pickup_longitude), weight: 3 };
            })
          );

          this._googleMap.heatmap = new google.maps.visualization.HeatmapLayer({
            data: this.state.heatMapData
          });

          this._googleMap.heatmap.setMap(this._googleMap.map_);
      }
    }
  }
}

  ensureDataFetched() {
    const date = new Date("2015", "01", "15");
    this.props.requestTaxiData(date);
  }

  onMarkerClick = (props, marker, e) =>
    this.setState({
      selectedPlace: props,
      activeMarker: marker,
      showingInfoWindow: true,
    });

  onMapClicked = (props) => {
    if (this.state.showingInfoWindow) {
      this.setState({
        showingInfoWindow: false,
        activeMarker: null,
      });
    }
  };

  onZoomChanged = (props) => {
    debugger;
  };

  location = {
    lat: 40.7128,
    lng: -73.935242,
    zoomLevel: 7,
  }; // our location object from earlier

  render() {
    return (
      <div className="map">
        <div className="google-map">
          <GoogleMapReact
            ref={(el) => this._googleMap = el} 
            bootstrapURLKeys={{
              key: "AIzaSyDK6592-PkejiR3LpQwDtSBh55FnKSWz0I",
            }}
            defaultCenter={this.location}
            defaultZoom={this.location.zoomLevel}
            heatmapLibrary={true}
            heatmap={this.state.heatMapData}
            onBoundsChange={this.onZoomChanged}
          ></GoogleMapReact>
        </div>
      </div>
    );
  }
}

export default connect(
  (state) => state.taxiData, // Selects which state properties are merged into the component's props
  TaxiDataStore.actionCreators // Selects which action creators are merged into the component's props
)(MapContainer);