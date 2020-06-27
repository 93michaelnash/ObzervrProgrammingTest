import * as React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

export default class Layout extends React.Component {
    static displayName = Layout.name;

    render() {
        return (
            <React.Fragment>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
            </React.Fragment>
        );
    }
}
