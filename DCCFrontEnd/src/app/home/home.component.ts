import { Component, OnInit, Inject, AfterContentInit } from '@angular/core';
import { faUserMd, faCapsules } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../_services/Auth.service';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, AfterContentInit {
  loading = true;
  ngAfterContentInit(): void {
    this.loading = false
  }
  registerMode = false;
  constructor(@Inject(DOCUMENT) private document: Document, public authService: AuthService) { }
  faUserMd = faUserMd;
  faCapsules = faCapsules;
  ngOnInit() {

  }
  registerToggle() {
    this.registerMode = true;
  }
  cancelRegisterMode(registermode: boolean) {
    this.registerMode = registermode;
  }
}
