import React from 'react';
import { Form } from 'react-bootstrap';
import PackageCheckbox from './package-checkbox';

export default class Subcategory extends React.Component {
    render() {
        const sdsetup = this.props.sdsetup;
        const subcategory = this.props.subcategory;
        const packages = subcategory.packages.map((id) => {
            const pkg = sdsetup.getPackageById(id);
            if (sdsetup.canShow(pkg)) {
                return (
                    <PackageCheckbox key={"pkg_"+pkg.id} sdsetup={sdsetup} pkg={pkg} />
                )
            } else return null;
        });
        return (
            <div className="subcategory">
                <h6 className="muted">{subcategory.name}</h6>
                <Form>
                {packages}
                </Form>
            </div>
        )
    }
}