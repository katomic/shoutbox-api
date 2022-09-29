import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'contacts',
  templateUrl: './contact.component.html'
})
export class ContactsComponent {
  public contacts: Contact[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Contact[]>(`${baseUrl}api/contact`).subscribe(result => {
      this.contacts = result;
    }, error => console.error(error));
  }
}

interface Contact {
  firstname: string;
  lastname: number;
  email: number;
}
