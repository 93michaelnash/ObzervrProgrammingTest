import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import authService from '../components/api-authorization/AuthorizeService'

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TaxiDataState {
    isLoading: boolean;
    taxiData: string[];
    date: Date;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTaxiDataAction {
    type: 'REQUEST_TAXI_DATA';
    date: Date;
}

interface ReceiveTaxiDataAction {
    type: 'RECEIVE_TAXI_DATA';
    date: Date;
    taxiData: string[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTaxiDataAction | ReceiveTaxiDataAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTaxiData: (date: Date): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        try{
            const appState = getState();
            // const token = await authService.getAccessToken();
            if (appState && appState.taxiData && date !== appState.taxiData.date) {
                fetch("https://localhost:5001/taxidata?date=" + date.toJSON())
                //, {
                //     headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
                // })
                    .then(response => response.json() as Promise<any>)
                    .then(data => {

                        dispatch({ type: 'RECEIVE_TAXI_DATA', date: date, taxiData: data });
                    });
    
                dispatch({ type: 'REQUEST_TAXI_DATA', date: date });
            }
        }
       catch(ex) {
           debugger;
       }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TaxiDataState = { taxiData: [], isLoading: false, date: null };

export const reducer: Reducer<TaxiDataState> = (state: TaxiDataState | undefined, incomingAction: Action): TaxiDataState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_TAXI_DATA':
            return {
                date: action.date,
                taxiData: state.taxiData,
                isLoading: true
            };
        case 'RECEIVE_TAXI_DATA':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            debugger;
            if (action.date === state.date) {
                return {
                    date: action.date,
                    taxiData: action.taxiData,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
