import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'companies',
  templateUrl: './company.component.html'
})
export class CompaniesComponent {
  public companies: Company[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Company[]>(`${baseUrl}api/company`).subscribe(result => {
      this.companies = result;
    }, error => console.error(error));
  }
}

interface Company {
  name: string;
  registrationDate: Date;
  active: boolean;
}
