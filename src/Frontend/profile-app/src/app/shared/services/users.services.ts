import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { catchError, map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { User } from '../models/user.model';
import { Pagination } from '../models/pagination.model';
import { ChangePassword } from '../models/changePassword.model';

@Injectable({ providedIn: 'root' })
export class UserServices extends BaseService {
    private _sharedHeaders = new HttpHeaders();

    constructor(private http: HttpClient) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');

    }

    update(id: string, entity: User) {
        return this.http.put(`${environment.apiUrl}/api/users/${id}`, JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    changePassword(id: string, entity: any) {
        return this.http.put(`${environment.apiUrl}/api/users/${id}/change-password`
        , JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }

    getDetail(id) {
        return this.http.get<User>(`${environment.apiUrl}/api/users/${id}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    delete(id) {
        return this.http.delete(environment.apiUrl + '/api/users/' + id, { headers: this._sharedHeaders })
            .pipe(
                catchError(this.handleError)
            );
    }

}