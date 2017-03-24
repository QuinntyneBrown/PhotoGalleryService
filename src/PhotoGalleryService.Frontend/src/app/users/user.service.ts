import { fetch, formEncode } from "../utilities";
import { User } from "./user.model";

export class UserService {

    private static _instance;

    public static get Instance() {
        this._instance = this._instance || new UserService();
        return this._instance;
    }

    public get() {
        return fetch({ url: "/api/user/get", authRequired: true });
    }

    public getById(id) {
        return fetch({ url: `/api/user/getbyid?id=${id}`, authRequired: true });
    }

    public add(user) {
        return fetch({ url: `/api/user/add`, method: "POST", data: { user }, authRequired: true });
    }

    public remove(options: { id: number }) {
        return fetch({ url: `/api/user/remove?id=${options.id}`, method: "DELETE", authRequired: true });
    }

    public tryToLogin = (options: { username: string, password: string }) => {
        Object.assign(options, { grant_type: "password" });
        return fetch({
            url: "/api/user/token",
            method: "POST",
            headers: { "Content-Type": "application/x-www-form-urlencoded" },
            data: formEncode(options)
        }).catch((error) => {

        });
    }

    public register(register: {
        firstname: string,
        lastname: string,
        email: string,
        password: string,
        confirmPassword: string,
        secret:string
    }) {
        return fetch({
            url: "/api/user/register",
            method: "POST",
            data: { register }
        });
    }

    public getCurrentUser() {
        return fetch({
            url: "/api/user/current",
            method: "GET",
            authRequired: true
        }).then((results: string) => {
            if (results == "")
                return null;

            return (<{ user: User }>JSON.parse(results)).user; 
        });
    }

}