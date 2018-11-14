import {Component, Inject} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public bookDetail: BookDetail;
  private isbn: string;
  private httpClient: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.bookDetail = {
      bib_key: '',
      thumbnail_url: '',
      preview_url: '',
      preview: '',
      info_url: ''
    };
  }

  onSubmit() {
    const params = new HttpParams().set('isbn', this.isbn);
    this.httpClient.get<BookDetail>(this.baseUrl + 'api/SampleData/BookDetailses', {params}).subscribe(result => {
      this.bookDetail = result;
    });
  }
}

interface BookDetail {
  bib_key: string;
  preview: string;
  thumbnail_url: string;
  preview_url: string;
  info_url: string;
}
