import { inject, Injectable } from "@angular/core";
import { UpdateRoleRequest, UserDto } from "../models/user.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";

@Injectable({providedIn: 'root'})
export class UserService{

    private http = inject(HttpClient);

    updateRole(userId: string, request: UpdateRoleRequest){
        return this.http.patch<UserDto>(
            `${environment.apiUrl}/User/${userId}/role`,
            request,
            {withCredentials:true}
        );
    }

    getAllUsersExceptAdmin(){
        return this.http.get<UserDto[]>(
            `${environment.apiUrl}/User/all`,
            {withCredentials:true},
        );
    }
}