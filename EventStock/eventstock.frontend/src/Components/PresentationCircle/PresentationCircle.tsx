import React from "react";
import "./PresentationCircle.css";

import partnerIcon1 from "./partners/partner_1.jfif";
import partnerIcon2 from "./partners/partner_2.jfif";
import partnerIcon3 from "./partners/partner_3.jfif";
import partnerIcon4 from "./partners/partner_4.jfif";
import partnerIcon5 from "./partners/partner_5.jfif";
import partnerIcon6 from "./partners/partner_6.jfif";
import partnerIcon7 from "./partners/partner_7.jfif";
import partnerIcon8 from "./partners/partner_8.jfif";


type Props = {};

const images = [partnerIcon1, partnerIcon2, partnerIcon3, partnerIcon4, partnerIcon5, partnerIcon6, partnerIcon7, partnerIcon8];

const PresentationCircle = (props: Props) => {
    return (       
        <>
            <div className="wrapper">
                <h2>Companies that trusted us:</h2>
                <div className="container">
                    {images.map((img, i) => (
                        <span key={i} style={{ "--i": i + 1 } as React.CSSProperties}>
                            <img src={img} alt={`PartnerLogo ${i + 1}`} />
                        </span>
                    ))}
                </div>
            </div>
        </>
    );
};

export default PresentationCircle;