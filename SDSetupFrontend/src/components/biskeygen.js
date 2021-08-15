import React from 'react';
import bsCustomFileInput from 'bs-custom-file-input';
import { Form, Tabs, Tab, Button } from 'react-bootstrap';
import deriveBisKeys from '../bis-api'; 

export default class BisKeyGen extends React.Component {
    constructor(props) {
        super(props);
        
        this.state = {
            results: []
        }
    }

    componentDidMount() {
        bsCustomFileInput.init();
    }

    deriveFromFile() {
        let fc = document.getElementById('file-fuse-cached').files[0];
        let tsec = document.getElementById('file-tsec-keys').files[0];
        let issues = [];

        if (fc && tsec) {
            if (fc.size !== 768) issues.push("Uploaded fuse_cached.bin is not the correct size.");
            if (tsec.size !== 32) issues.push("Uploaded tsec_keys.bin is not the correct size.");

            if (issues.length > 0) {
                this.showResults(issues);
            } else {
                Promise.all([fc.arrayBuffer(), tsec.arrayBuffer()]).then((results) => {
                    let res;
                    let fc = new Uint8Array(results[0]);
                    let tsec = new Uint8Array(results[1]);
    
                    if (fc[0x10] !== 0x83) {
                        issues.push("Your fuse_cached.bin is not valid.");
                    }
        
                    if (issues.length > 0) {
                        this.showResults(issues);
                    } else {
                        let sbkKey = fc.slice(0xA4, 0xA4 + 0x10);
                        let tsecKey = tsec.slice(0x0, 0x10);
                        res = deriveBisKeys(sbkKey, tsecKey);
                        this.showResults(res);
                    }
                });
            }
        }

    }

    deriveFromKeys() {
        const validChars = ['a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        let res = [];
        let sbk = document.getElementById('key-sbk').value;
        let tsec = document.getElementById('key-tsec').value;

        if (sbk.length != 32) res.push("SBK key length is not valid.");
        if (tsec.length != 32) res.push("TSEC key length is not valid.");
        sbk.split('').every((x) => {
            if (!validChars.includes(x)) res.push("SBK key contains invalid characters.");
            return validChars.includes(x);
        });
        tsec.split('').every((x) => {
            if (!validChars.includes(x)) res.push("TSEC key contains invalid characters.");
            return validChars.includes(x);
        });

        if (res.length > 0) {
            this.showResults(res);
        } else {
            res = deriveBisKeys(sbk, tsec);
            this.showResults(res);
        }
    }    

    showResults(arr) {
        this.setState({results: arr});
    }

    render() {
        const results = this.state.results.join('\n');

        return (
            <div>
                <div className="center">
                    <h1 className="tall-margin">
                        BIS Key Calculator
                    </h1>
                    <div>Key deriver based on an implementation by Simon.</div>
                    <div>All operations are performed within your browser. No key data is ever sent to a server.</div>
                    <div>You can calculate your BIS keys below by either uploading a couple Hekate dumps, or by entering your SBK and TSEC keys manually.</div>
                </div>

                <div className="bis-box">
                    <Tabs defaultActiveKey="bis-files">
                        <Tab eventKey="bis-files" title="Upload Dumps">
                            <Form>
                                <h6>Upload Hekate fuse_cached.bin</h6>
                                <Form.File id="file-fuse-cached" label="No file selected" custom />

                                <h6>Upload Hekate tsec_keys.bin</h6>
                                <Form.File id="file-tsec-keys" label="No file selected" custom />
                            </Form>
                            <br/>
                            <Button variant="danger" onClick={() => {this.deriveFromFile()}} className="btn-wide">Derive BIS keys from files</Button>
                        </Tab>
                        <Tab eventKey="bis-text" title="Enter Keys">
                            <Form>
                                <h6>Enter SBK Key</h6>
                                <Form.Control id="key-sbk" placeholder="00000000000000000000000000000000"  />

                                <h6>Enter TSEC Key</h6>
                                <Form.Control id="key-tsec" placeholder="00000000000000000000000000000000" />
                            </Form>
                            <br/>
                            <Button variant="danger" onClick={() => {this.deriveFromKeys()}} className="btn-wide">Derive BIS keys from SBK/TSEC keys</Button>
                        </Tab>
                    </Tabs>
                    
                    <Form.Control as="textarea" rows={10} readOnly value={results} />

                </div>
            </div>
        );
    }
}