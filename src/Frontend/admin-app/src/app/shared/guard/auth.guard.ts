import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { async } from '@angular/core/testing';

@Injectable()
export class AuthGuard implements CanActivate {

  role: string;
  constructor(private router: Router, private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isAuthenticated()) {
      if (this.loadRole()) {
        return true;
      }
      this.router.navigate(['/access-denied']);
      return false;
    }
    this.router.navigate(['/login'], { queryParams: { redirect: state.url }, replaceUrl: true });
    return false;
  }

  loadRole() {
    const profile = this.authService.profile;
    if (profile.role === 'Admin') { return true; }
    localStorage.clear();
    sessionStorage.clear();
  }
}