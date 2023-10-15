using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Mapper
{
    public static class BoardMapper
    {
        public static Board ConvertToLogic(this BoardDTO boardDTO)
        {
            if(boardDTO == null)
                return new Board();

            Board b = new Board();
            b.Id = boardDTO.Id;
            b.name = boardDTO.name;
            foreach(AudiofileDTO audiofile in boardDTO.AudioList)
            {
                b.AudioList.Add(audiofile.ConvertToLogic());
            }
            return b;
            //return new Board()
            //{
            //    Id = boardDTO.Id,
            //    name = boardDTO.name
            //};
        }
        public static BoardDTO ConvertToDTO(this Board board) 
        {
            if (board == null)
                return new BoardDTO();


            return new BoardDTO()
            {
                Id = board.Id,
                name = board.name,  
            };
        }
    }
}
