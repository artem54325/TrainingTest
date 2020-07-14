using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrainingTests.Models;
using TrainingTests.Repositories;

namespace TrainingTests.Helpers
{
    public class ClickLikeHelpers
    {
        private readonly MySqlContext _context;

        public ClickLikeHelpers(MySqlContext context)
        {
            _context = context;
        }

        public Article ShowArticle(string idArticle)
        {
            Article article = GetActicle(idArticle);
            article.Viewer += 1;

            _context.Articles.Update(article);
            _context.SaveChanges();

            return article;
        }

        public Article ClickLikeArticle(string user, string idArticle)
        {
            Article article = GetActicle(idArticle);

            if (article.LikeUsers == null)
            {
                article.LikeUsers = new System.Collections.Generic.List<string>();
            }

            if (!article.LikeUsers.Contains(user))
            {
                article.LikeUsers.Add(user);
            }
            else
            {
                article.LikeUsers.Remove(user);
            }


            _context.Articles.Update(article);
            _context.SaveChanges();

            return article;
        }

        public Comment ClickLikeComment(string user, string commentId)
        {
            Comment comment = GetComment(commentId);

            if (comment.LikeUsers == null)
            {
                comment.LikeUsers = new System.Collections.Generic.List<string>();
            }

            if (!comment.LikeUsers.Contains(user))
            {
                comment.LikeUsers.Add(user);
            }
            else
            {
                comment.LikeUsers.Remove(user);
            }

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return comment;
        }

        private Comment GetComment(string id)
        {
            return _context.Comments.Include(a => a.UserCreate).FirstOrDefault(a => a.Id.Equals(id));
        }

        private Article GetActicle(string id)
        {
            return _context.Articles.Include(article => article.Comments).ThenInclude(a => a.UserCreate)
                .Include(a => a.UserCreate).FirstOrDefault(a => a.Id.Equals(id));
        }
    }
}
