import React from 'react';
import { Link } from 'react-router-dom';
import { Card, CardGroup } from 'react-bootstrap';
import logo from '../img/logo_big.png'

class Home extends React.Component {
    render() {
        const sdsetup = this.props.sdsetup;
        const platforms = sdsetup.getAllPlatforms();
        let platformCards = platforms.map((key) => {
            const platform = sdsetup.getPlatformById(key);
            return (
                <Card className="platform-card">
                    <Link to={"/console/"+key}>
                        <Card.Img src={platform.homeIcon} />
                    </Link>
                </Card>
            );
        });
        
        return (
            <div className="home">
                <img className="big-logo d-none d-sm-block" src={logo} alt="Logo"/>
                <Card>
                    <Card.Body>
                        <Card.Text>
                            SDSetup is a web application which makes setting up homebrew easier. 
                            Simply select the homebrew and custom firmwares you want,
                            and quickly create a zip archive to extract to your SD card. 
                            The Ninite for your game consoles!
                        </Card.Text>
                    </Card.Body>
                </Card>
                <br/>
                <Card>
                    <Card.Body>
                        <Card.Title>Select a Console</Card.Title>
                        <CardGroup className="platform-card-group">
                            {platformCards}
                        </CardGroup>
                    </Card.Body>
                </Card>
            </div>
        );
    }
}

export default Home;