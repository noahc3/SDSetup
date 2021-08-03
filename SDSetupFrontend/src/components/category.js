import React from 'react';
import { Card } from 'react-bootstrap';
import Subcategory from './subcategory';

export default class Category extends React.Component {
    render() {
        const sdsetup = this.props.sdsetup;
        const category = this.props.category;
        const subcategories = Object.keys(category.subcategories).map((key) => {
            const subcategory = category.subcategories[key];
            if (sdsetup.canShow(subcategory)) {
                return (
                    <Subcategory key={"subcat_"+subcategory.id} sdsetup={sdsetup} subcategory={subcategory}/>
                )
            } else return null;
        });
        return (
            <Card>
                <Card.Body>
                    <Card.Title>{category.name}</Card.Title>
                    {subcategories}
                </Card.Body>
            </Card>
        )
    }
}