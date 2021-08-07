import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Switch, Link } from 'react-router-dom';
import { Navbar, Nav, Spinner, Container } from 'react-bootstrap';
import Home from './components/home';
import Console from './components/console';

import * as sdsetup from './sdsetup-api';

import 'bootstrap/dist/css/bootstrap.css';
import './index.css';

import logo from './img/logo_nav.png'


function SiteNavbar() {
    const platformLinks = sdsetup.getAllPlatforms().map((id) => {
        const platform = sdsetup.getPlatformById(id);
        return (
            <Nav.Link key={"nav_console_" + platform.id} as={Link} to={"/console/" + id}>{platform.menuName}</Nav.Link>
        );
    });
    return (
        <Navbar bg="danger" variant="dark" expand="lg">
            <Navbar.Brand as={Link} to="/"><img src={logo} alt="Logo"/></Navbar.Brand>
            <Navbar.Toggle aria-controls="site-navbar" />
            <Navbar.Collapse id="site-navbar">
                <Nav className="mr-auto">
                    {platformLinks}
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    )
}

class App extends React.Component {
    constructor(props) {
        super(props);
        sdsetup.setForceUpdate(() => {
            this.forceUpdate();
        });
        window.sdsetup = sdsetup;
        this.state = {
            ready: false,
            manifest: null,
        }

        sdsetup.fetchLatestManifest().then(() => {
            this.setState({ready: true});
        });
    }

    render() {
        if (!this.state.ready) {
            return (
                <main>
                    <div className="loading-splash">
                        <div>
                            <Spinner animation="border" variant="light" />
                            <h4>Loading</h4>
                        </div>
                    </div>
                </main>
            )
        } else {
            return (
                <main>
                    <SiteNavbar/>
                    <Container>
                        <Switch>
                            <Route path='/' exact>
                                <Home />
                            </Route>
                            <Route path='/console/:platformid' component={
                                props => <Console platformid={props.match.params.platformid} />
                            }>
                            </Route>
                        </Switch>
                    </Container>
                </main>
            )
        }
    }

    
}


ReactDOM.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
    document.getElementById('root')
);