import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  errorMessage!: string;
  authForm!: FormGroup;
  loading: boolean = false;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) {
    this.SetAuthenticationForm();
  }

  private SetAuthenticationForm() {
    this.authForm = this.formBuilder.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: false
    });
  }

  ngOnInit(): void {
  }

  signIn(user: User){
    if (!this.authForm.valid) return;
    this.loading = true;
    return this.authService.signIn(user).subscribe(
      user => this.HandleOkCase(user),
      error => this.HandleErrorCase(error)
    );
  }

  private HandleOkCase(user: User) {
    this.loading = false;
    localStorage.setItem('user', JSON.stringify(user));
    this.router.navigateByUrl('/');
  }

  private HandleErrorCase(error: any) {
    console.log(error);
    this.errorMessage = "Login failed. Please try again!";
    this.loading = false;
  }
}