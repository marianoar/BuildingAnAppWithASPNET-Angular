import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit{

  members$: Observable<Member[]> | undefined;


  constructor(private memberService: MembersService) {} 
  

  ngOnInit(): void {
    // this.loadMembers();
    // console.log("members list: ",this.members);
    this.members$ = this.memberService.getMembers();
  }

  // loadMembers(){
  //   this.memberService.getMembers().subscribe(

  //   //   {next: members => this.members = members
      
  //   //     }
  //   data =>
  //     {
  //       console.log("datta ",data);
  //       this.members=data;
  //   }
  // )
  // }

}
