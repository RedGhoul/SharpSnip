import { Injectable, EventEmitter } from '@angular/core';
import { Notes } from '@app/Models/Note.model';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class NotesService {
  notesLoaded = new EventEmitter<Notes[]>();
  private notes: Notes[];
  constructor(private http: HttpClient) {
    this.http.get<Notes[]>("https://localhost:5001/api/Notes").subscribe(
      (notes: Notes[]) => {
        this.notes = notes;
        this.getNotes();
      }
    )
  }

  getNotes() {
    this.notesLoaded.emit(this.notes);
  }
}
