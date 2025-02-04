using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Models.Command.Blog.Like;
public class DeleteLikeCommand
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}
