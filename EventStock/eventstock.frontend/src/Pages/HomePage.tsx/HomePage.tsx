import { Outlet } from "react-router-dom";
import Sidebar from "../../Components/Sidebar/Sidebar";

type Props = {};

const HomePage = (props: Props) => {
    return (
        <>
            <Sidebar />
            <div className="relative md:ml-64 bg-blueGray-100 w-full">
                <div className="relative pt-20 pb-32 bg-lightBlue-500">
                    <div className="px-4 md:px-6 mx-auto w-full">
                        <div className="flex flex-wrap">{<Outlet />}</div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default HomePage;