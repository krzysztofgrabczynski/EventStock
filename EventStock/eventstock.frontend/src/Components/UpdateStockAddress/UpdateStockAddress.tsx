import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../Context/useAuth";
import { Address } from "../../Models/Stock/Address";
import { UpdateStockAddress as UpdateStockAddressModel } from "../../Models/Stock/UpdateStockAddress"
import { editStockAddressAPI } from "../../Api/apiStock";

type Props = {}

type UpdateStockAddressForm = {
    country: string;
    city: string;
    zipCode: string;
    street: string;
    buildingNumber: string;
    flatNumber: number | undefined;
}

const UpdateStockAddress = (props: Props) => {
    const { user } = useAuth();
    const formSchema = Yup.object().shape({
        country: Yup.string().required("Country field is required"),
        city: Yup.string().required("City field is required"),
        zipCode: Yup.string().required("Zip code field is required"),
        street: Yup.string().required("Street field is required"),
        buildingNumber: Yup.string().required("Building number field is required"),
        flatNumber: Yup.number().nullable().notRequired(),
    });

    const { register, handleSubmit, formState: { errors } } = useForm<UpdateStockAddressForm>({
        resolver: yupResolver(formSchema)
    });

    const onSubmit = async (data: UpdateStockAddressForm) => {
        console.log("Form Data:", data);
        const address: Address = {
            country: data.country,
            city: data.city,
            zipCode: data.zipCode,
            street: data.street,
            buildingNumber: data.buildingNumber,
            flatNumber: data.flatNumber,
        };
        const updateStockAddressRequest: UpdateStockAddressModel = {
            stockId: user.stockId,
            address: address,
        };
        await editStockAddressAPI(updateStockAddressRequest);
    };

    return (
        <div className="p-6">
            <h2 className="text-2xl font-bold mb-4">Update Stock Address</h2>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div>
                    <label htmlFor="country" className="block">Country</label>
                    <input
                        id="country"
                        {...register("country")}
                        className="border p-2 rounded"
                    />
                    {errors.country && <span className="text-red-500">{errors.country.message}</span>}
                </div>

                <div>
                    <label htmlFor="city" className="block">City</label>
                    <input
                        id="city"
                        {...register("city")}
                        className="border p-2 rounded"
                    />
                    {errors.city && <span className="text-red-500">{errors.city.message}</span>}
                </div>

                <div>
                    <label htmlFor="zipCode" className="block">Zip Code</label>
                    <input
                        id="zipCode"
                        {...register("zipCode")}
                        className="border p-2 rounded"
                    />
                    {errors.zipCode && <span className="text-red-500">{errors.zipCode.message}</span>}
                </div>

                <div>
                    <label htmlFor="street" className="block">Street</label>
                    <input
                        id="street"
                        {...register("street")}
                        className="border p-2 rounded"
                    />
                    {errors.street && <span className="text-red-500">{errors.street.message}</span>}
                </div>

                <div>
                    <label htmlFor="buildingNumber" className="block">Building Number</label>
                    <input
                        id="buildingNumber"
                        {...register("buildingNumber")}
                        className="border p-2 rounded"
                    />
                    {errors.buildingNumber && <span className="text-red-500">{errors.buildingNumber.message}</span>}
                </div>

                <div>
                    <label htmlFor="flatNumber" className="block">Flat Number (Optional)</label>
                    <input
                        id="flatNumber"
                        type="number"
                        {...register("flatNumber")}
                        className="border p-2 rounded"
                    />
                    {errors.flatNumber && <span className="text-red-500">{errors.flatNumber.message}</span>}
                </div>

                <button type="submit" className="mt-4 px-6 py-2 bg-blue-500 text-white rounded hover:bg-blue-700">
                    Update Address
                </button>
            </form>
        </div>
    );
};

export default UpdateStockAddress;
