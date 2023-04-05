import { IUser } from "./user";

export interface IBooking{
    id: number;
    week: number;
    dayOfWeek: number;
    hour: number;
    user: IUser;
}