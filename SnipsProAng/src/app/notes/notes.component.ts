import { Component, OnInit } from '@angular/core';
import { NotesService } from '@app/Services/notes.service';
import { Notes } from '@app/Models/Note.model';

@Component({
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.less']
})
export class NotesComponent implements OnInit {

  curNotes: Notes[];
  constructor(private notesService: NotesService) { }

  ngOnInit(): void {
    console.log("ngOnInit")
    this.notesService.notesLoaded.subscribe((notes: Notes[]) => {
      this.curNotes = notes;
      console.log(this.curNotes)
    })
    if (this.curNotes == null) {
      this.notesService.getNotes();
    }
  }

  viewNotes() {
    console.log(this.curNotes)
  }

}
