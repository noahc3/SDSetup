import React from 'react';
import { Alert, Card, CardGroup, Button, Modal, ProgressBar } from 'react-bootstrap';
import PreconfiguredBundleCard from './preconfigured-bundle-card';
import CheckboxSection from './checkbox-section';
import parse from 'html-react-parser';

class DownloadModalContents extends React.Component {
    constructor(props) {
        super(props);
        const sdsetup = props.sdsetup;
        sdsetup.setModalUpdate(() => { this.forceUpdate(); });
    }

    render() {
        const sdsetup = this.props.sdsetup;
        const bundlerProgress = sdsetup.getBundlerProgress();

        return (
                <div>
                    <Modal.Header>
                        <Modal.Title>Your bundle is being prepared, please wait.</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div class="center">
                            <h5>{bundlerProgress.currentTask}</h5>
                            <p>({bundlerProgress.progress}/{bundlerProgress.total})</p>
                            <ProgressBar variant="success" animated now={bundlerProgress.progress} max={bundlerProgress.total} />
                        </div>
                    </Modal.Body>
                </div>
        )
    }
}

class Console extends React.Component {

    render() {
        const sdsetup = this.props.sdsetup;
        const isBundling = sdsetup.isBundling();
        const copyright = sdsetup.getCopyrightText();
        const platformid = this.props.platformid;
        const platform = sdsetup.getPlatformById(platformid);

        const bundles = sdsetup.getBundlesForPlatform(platformid).map((bundle) => {
            return (
                <PreconfiguredBundleCard key={"bundle_"+bundle.name} sdsetup={sdsetup} bundle={bundle} />
            );
        });

        const sections = Object.keys(platform.packageSections).map((id) => {
            const section = platform.packageSections[id];
            if (sdsetup.canShow(section)) {
                return (
                    <div key={"section_"+section.id}>
                        <CheckboxSection sdsetup={sdsetup} section={section} />
                        <br />
                    </div>
                )
            } else return null;
        });

        const downloadingModal = (
            <Modal key="downloading-modal" show={isBundling} size="lg" centered>
                <DownloadModalContents sdsetup={sdsetup}/>
            </Modal>
        );

        return (
            <div>
            {downloadingModal}
                <h1 className="center">
                    {platform.name}
                </h1>
                <Alert key={platformid+"_topalert"} variant="primary" className="center">
                    Want to hack your Switch but not sure what to do? 
                    Check out the official SDSetup guide: <a href="https://switch.homebrew.guide">https://switch.homebrew.guide</a>
                </Alert>
                <Card>
                    <Card.Body>
                        <Card.Title>Pre-configured bundles</Card.Title>
                        <CardGroup>
                            {bundles}
                        </CardGroup>
                    </Card.Body>
                </Card>
                <br />
                {sections}
                <Button onClick={() => { sdsetup.requestBundle(platformid); }} variant="danger" size="lg" block>Get My Bundle</Button>
                <br />
                <div class="copyright">
                    {parse(copyright)}
                </div>
            </div>
        );
    }
}

export default Console;