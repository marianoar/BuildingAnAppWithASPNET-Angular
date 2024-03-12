import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css'],
})
export class TestErrorComponent {
  baseURL = 'http://localhost:5249/api/';

  validationErrors: string[] = [];

  /**
   *
   */
  constructor(private http: HttpClient) {}

  get404error() {
    this.http.get(this.baseURL + 'buggy/not-found').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get400error() {
    this.http.get(this.baseURL + 'buggy/bad-request').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get500error() {
    this.http.get(this.baseURL + 'buggy/server-error').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get401error() {
    this.http.get(this.baseURL + 'buggy/auth').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get400ValidationError() {
    this.http.post(this.baseURL + 'account/register', {}).subscribe({
      next: (response) => console.log(response),
      error: (error) => {
        console.log(error);
        this.validationErrors = error;
      },
    });
  }
}
