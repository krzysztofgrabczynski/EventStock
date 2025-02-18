import { useEffect, useState } from "react";
import { Stock } from "../../Models/Stock/Stocks";
import { myStockAPI } from "../../Api/apiUser";

type Props = {};

const UserStocks = (props: Props) => {
    const [stock, setStock] = useState<Stock[]>([]);

    const fetchData = async () => {
        const response = await myStockAPI();
        setStock(response.data);
    }

    useEffect(() => {
        fetchData();
    }, []);

    return (
        <div className="p-6">
            {stock ? (
                <div className="bg-white shadow-lg rounded-lg p-4 border border-gray-200">
                    <h3 className="text-xl font-semibold text-blue-600">{stock.name}</h3>
                    {stock.address ? (
                        <>
                            <p className="text-gray-600">Country: <span className="font-bold">{stock.address.country}</span></p>
                            <p className="text-gray-600">City: <span className="font-bold">{stock.address.city}</span></p>
                            <p className="text-gray-600">Zip code: <span className="font-bold">{stock.address.zipCode}</span></p>
                            <p className="text-gray-600">Street: <span className="font-bold">{stock.address.street}</span></p>
                            <p className="text-gray-600">Building no: <span className="font-bold">{stock.address.buildingNumber}</span></p>
                            {stock.address.flatNumber && (
                                <p className="text-gray-600">Flat no: <span className="font-bold">{stock.address.flatNumber}</span></p>
                            )}
                        </>
                    ) : (
                        <p className="text-gray-600">No address available</p>
                    )}
                </div>
            ) : (
                <p className="text-gray-600">No stock available</p>
            )}
        </div>
    );
};

export default UserStocks;
