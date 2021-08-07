import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Switch, Link } from 'react-router-dom';
import { Navbar, Nav, Spinner, Container, Button, ButtonGroup, SafeAnchor } from 'react-bootstrap';
import Home from './components/home';
import Console from './components/console';

import * as utils from './utils';
import * as sdsetup from './sdsetup-api';
import './sdsetup-typedef';

import 'bootstrap/dist/css/bootstrap.css';
import './index.css';

import logo from './img/logo_nav.png'
import failIcon from './img/fail.png'

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

    /**
     * @typedef {{
     * ready: boolean,
     * error: ApiError
     * }} State
     */

    constructor(props) {
        super(props);

        sdsetup.setDefaultErrorHandler((err) => { this.handlePreInitError(err); });
        sdsetup.setForceUpdate(() => {
            this.forceUpdate();
        });
        window.sdsetup = sdsetup;

        /** @type {State} */
        this.state = {
            ready: false,
            error: undefined
        }

        sdsetup.fetchLatestManifest().then(() => {
            if (!this.state.error) this.setState({ready: true});
        });
    }

    handlePreInitError(err) {
        this.setState({error: err});
    }

    render() {
        if (this.state.error) {
            const err = this.state.error;
            const copyText = "```\n" + JSON.stringify(err,null,4) + "\n```";
            return (
                <main>
                    <div className="loading-splash">
                        <div>
                            <div className="center">
                                <img alt="" className="fail-icon" src={failIcon}/>
                                <h3>SDSetup failed to load</h3>
                                <p className="muted">Error {err.code}: {err.message}</p>
                                <p>This error was unexpected. If you have the time, please report this error by creating an issue on GitHub, and copy the following text into your report:</p>
                            </div>
                            <div className="error-box">
                                <pre>{copyText}</pre>
                                
                                <ButtonGroup className="btn-group-wide sharp-corners" vertical>
                                    <Button variant="secondary" onClick={() => { utils.copyTextToClipboard(copyText); }}>Copy to Clipboard</Button>
                                    <Button as={SafeAnchor} href="https://github.com/noahc3/sdsetup/issues" target="_blank" rel="noopener noreferrer" variant="danger">Go to GitHub</Button>
                                </ButtonGroup>
                            </div>
                        </div>
                    </div>
                </main>
            );
        } else if (!this.state.ready) {
            return (
                <main>
                    <div className="loading-splash">
                        <div className="center">
                            <Spinner animation="border" variant="light" />
                            <h3>Loading</h3>
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