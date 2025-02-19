export interface Address {
    id: number;
    country: string;
    city: string;
    zipCode: string;
    street: string;
    buildingNumber: string;
    flatNumber: number | undefined;
}
