import React from 'react';
import { Card } from 'react-bootstrap';
import Category from './category';

export default class CheckboxSection extends React.Component {
    render() {
        const sdsetup = this.props.sdsetup;
        const section = this.props.section;
        const categories = Object.keys(section.categories).map((key) => {
            const category = section.categories[key];
            if (sdsetup.canShow(category)) {
                return (
                    <div key={"cat_"+category.id} className="col-mb-4 category-col">
                        <Category sdsetup={sdsetup} category={category} />
                    </div>
                )
            } else return null;
        });
        return (
            <Card>
                <Card.Body>
                    <Card.Title>{section.name}</Card.Title>
                    <div className="row row-cols-1 row-cols-sm-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-3">
                        {categories}
                    </div>
                    
                </Card.Body>
            </Card>
        )
    }
}